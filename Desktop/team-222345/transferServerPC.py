#1.Сваляме списък с users, след което избираме user и сваляме информцията за него в zip файл
import requests
import zipfile
import json
import io
from pathlib import Path


PROJECTS_URL = "http://192.168.8.85:8000/api/v1/reports/get_all_metadata/"
DOWNLOADS_BASE = "http://192.168.8.85:8000/api/v1/reports/download/?archive-id="

#взима users
def get_all_users():
    response = requests.get(PROJECTS_URL, timeout=20)
    response.raise_for_status()
    data = response.json()
    
    print("\n(ID|Proj|P/F/S/E|Status):")
    print("-" * 32)  # Максимална ширина за малък дисплей
    
    for i, user in enumerate(data, 1):
        user_id = str(user.get('id'))[:4]  # Ограничаваме ID до 4 символа
        project = user.get('project_name', 'N/A')[:3]  # Само първите 3 букви от проекта
        bu = user.get('user_bu', 'N/A')[:2]  # Само 2 букви за BU
        
        try:
            with requests.get(f"{DOWNLOADS_BASE}{user_id}", stream=True, timeout=15) as response:
                response.raise_for_status()
                zip_buffer = io.BytesIO(response.content)
                
                with zipfile.ZipFile(zip_buffer) as z:
                    counts = {'p':0, 'f':0, 's':0, 'e':0}
                    for f in z.namelist():
                        if f.endswith('.json'):
                            with z.open(f) as json_file:
                                tests = json.load(json_file).get('test_suite', [])
                                for test in tests:
                                    status = list(test.values())[0].get('test_case_status','').lower()[0]
                                    if status in counts:
                                        counts[status] += 1
                    
                    total = sum(counts.values())
                    if total > 0:
                        test_str = f"{counts['p']}/{counts['f']}/{counts['s']}/{counts['e']}"
                        status = "P" if counts['f'] == 0 and counts['e'] == 0 else "F"  # P for Pass, F for Fail
                    else:
                        test_str = "0/0/0/0"
                        status = "N"  # N for No Tests
        except Exception:
            test_str = "E/E/E/E"  # E for Error
            status = "E"  # E for Error
        
        # Форматиране за малък дисплей (макс 32 символа)
        line = f"{i:2}.{user_id}|{project}|{test_str}|{status}"
        print(line[:32])  # Гарантираме че не надхвърляме ширината
    
    return data
#избор на user псоледно id

def choose_user(users):
    return users[-1]["id"]

#избор на user
def choose_user_io(users):
    ids = [u["id"] for u in users]
    print("Available user IDs:"+ " " + ", ".join(map(str, ids)))
    while True:
        choice = input("Choose a user ID from ids: ").strip()
        if choice.isdigit() and int(choice) in ids:
            return int(choice)
        else:
            print(f"Invalid choice. Please choose from {ids}.")
            
#изтегляне на zip файл
def download_zip(user_id, dest_folder=Path(".")):
    #адрес от кото ще теглим zip файла
    url = f"{DOWNLOADS_BASE}{user_id}"
    print(f"Downloading zip file for user {user_id} from {url}")
    response = requests.get(url,stream=True,timeout=30)
    response.raise_for_status()  # Raise an error for bad responses
    zip_path =dest_folder / f"{user_id}.zip"
    with zip_path.open("wb") as f:
        for chunk in response.iter_content(chunk_size=8192):
            f.write(chunk)
    print(f"Zip file downloaded to {zip_path}")
    return zip_path
#разархивиране на zip файла

def extract_and_show(zip_path, extract_to=None):
    extract_to = extract_to or zip_path.with_suffix('')  # Extract to the same directory as the zip file
    with zipfile.ZipFile(zip_path, 'r') as z:
        z.extractall(extract_to)
        print(f"Extracted files to {extract_to.resolve()}")
        # Показваме съдържанието
        for member in z.namelist():
            if member.endswith('.json'):
                with (extract_to / member).open('r') as fp:
                    data = json.load(fp)
                print(f"\nContents of {member}:")
                
                # Обработка на тестовите резултати
                if isinstance(data, dict) and 'test_suite' in data:
                    for test_item in data['test_suite']:
                        for test_name, test_data in test_item.items():
                            status = test_data.get('test_case_status', 'unknown')
                            print(f"{test_name}: {status}")
                elif isinstance(data, dict):
                    # Ако структурата е различна
                    for test_name, test_data in data.items():
                        if isinstance(test_data, dict):
                            status = test_data.get('test_case_status', 'unknown')
                            print(f"{test_name}: {status}")
                        else:
                            print(f"{test_name}: {test_data}")
                elif isinstance(data, list):
                    for item in data:
                        if isinstance(item, dict):
                            for test_name, test_data in item.items():
                                if isinstance(test_data, dict):
                                    status = test_data.get('test_case_status', 'unknown')
                                    print(f"{test_name}: {status}")
                                else:
                                    print(f"{test_name}: {test_data}")

def main():
    users = get_all_users()
    user_id = choose_user_io(users)
    zip_path = download_zip(user_id, Path("."))
    extract_and_show(zip_path)
if __name__ == "__main__":
    main()

