# ItransferServerPC.py
import requests
import zipfile
import json
import io
from pathlib import Path

PROJECTS_URL = "http://192.168.8.85:8000/api/v1/reports/get_all_metadata/"
DOWNLOADS_BASE = "http://192.168.8.85:8000/api/v1/reports/download/?archive-id="

def get_all_users():
    response = requests.get(PROJECTS_URL, timeout=20)
    response.raise_for_status()
    return response.json()

def download_zip(user_id, dest_folder=Path(".")):
    url = f"{DOWNLOADS_BASE}{user_id}"
    print(f"Downloading zip file for user {user_id} from {url}")
    response = requests.get(url, stream=True, timeout=30)
    response.raise_for_status()

    zip_path = dest_folder / f"{user_id}.zip"
    with zip_path.open("wb") as f:
        for chunk in response.iter_content(chunk_size=8192):
            f.write(chunk)

    return zip_path

def extract_results(zip_path, extract_to=None):
    extract_to = extract_to or zip_path.with_suffix('')
    extracted_data = {}

    with zipfile.ZipFile(zip_path, 'r') as z:
        z.extractall(extract_to)

        for member in z.namelist():
            if member.endswith('.json'):
                with (extract_to / member).open('r') as fp:
                    data = json.load(fp)
                test_results = []

                if isinstance(data, dict) and 'test_suite' in data:
                    for test_item in data['test_suite']:
                        for test_name, test_data in test_item.items():
                            status = test_data.get('test_case_status', 'unknown')
                            test_results.append((test_name, status))
                elif isinstance(data, dict):
                    for test_name, test_data in data.items():
                        if isinstance(test_data, dict):
                            status = test_data.get('test_case_status', 'unknown')
                        else:
                            status = test_data
                        test_results.append((test_name, status))
                elif isinstance(data, list):
                    for item in data:
                        if isinstance(item, dict):
                            for test_name, test_data in item.items():
                                if isinstance(test_data, dict):
                                    status = test_data.get('test_case_status', 'unknown')
                                else:
                                    status = test_data
                                test_results.append((test_name, status))

                extracted_data[member] = test_results

    return extracted_data
