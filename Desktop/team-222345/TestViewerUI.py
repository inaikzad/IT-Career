# TestViewerUI.py
import tkinter as tk
from tkinter import ttk, messagebox
from ItransferServerPC import get_all_users, download_zip, extract_results

# Table text (can also be generated dynamically if needed)
user_table = """(ID|Proj|P/F/S/E|Status):
--------------------------------
 1.1|OPE|2/1/2/2|F
 2.2|CPX|3/2/0/2|F
 3.3|VPP|2/2/1/0|F
 4.4|OPE|2/4/0/1|F
 5.5|CPX|0/2/1/2|F
 6.6|CPX|1/1/1/2|F
Available user IDs: 1, 2, 3, 4, 5, 6
P = Passed
F = Failed
S = Skipped
E = Error
"""

class TestViewerApp:
    def __init__(self, master):
        self.master = master
        master.title("Test Viewer")

        self.user_list = []
        self.current_user_id = None

        self.main_frame = tk.Frame(master)
        self.result_frame = tk.Frame(master)

        self.build_main_frame()
        self.build_result_frame()

        self.main_frame.pack()

        self.load_users()

    def build_main_frame(self):
        # Table
        table_label = tk.Label(self.main_frame, text=user_table, font=("Courier", 10), justify="left", anchor="w")
        table_label.pack(padx=10, pady=(10, 5), anchor="w")

        # Dropdown and button
        frame_select = ttk.Frame(self.main_frame)
        frame_select.pack(pady=10)

        label = ttk.Label(frame_select, text="Choose User ID:")
        label.pack(side=tk.LEFT)

        self.user_combobox = ttk.Combobox(frame_select, state="readonly")
        self.user_combobox.pack(side=tk.LEFT, padx=5)

        select_button = ttk.Button(frame_select, text="View Results", command=self.select_user)
        select_button.pack(side=tk.LEFT)

    def build_result_frame(self):
        self.results_text = tk.Text(self.result_frame, width=60, height=20)
        self.results_text.pack(padx=10, pady=10)

        self.back_button = ttk.Button(self.result_frame, text="Return", command=self.return_to_main)
        self.back_button.pack(pady=(0, 10))

    def load_users(self):
        try:
            self.user_list = get_all_users()
            ids = [str(user['id']) for user in self.user_list]
            self.user_combobox['values'] = ids
        except Exception as e:
            messagebox.showerror("Error", f"Failed to load users:\n{e}")

    def select_user(self):
        user_id = self.user_combobox.get()
        if not user_id:
            messagebox.showwarning("Select User", "Please choose a User ID.")
            return

        try:
            zip_path = download_zip(user_id)
            extracted_data = extract_results(zip_path)
            self.show_results(user_id, extracted_data)
        except Exception as e:
            messagebox.showerror("Error", f"Failed to retrieve results:\n{e}")

    def show_results(self, user_id, extracted_data):
        self.main_frame.pack_forget()
        self.result_frame.pack()

        self.results_text.delete(1.0, tk.END)
        self.results_text.insert(tk.END, f"Results for User ID {user_id}:\n\n")

        for filename, tests in extracted_data.items():
            self.results_text.insert(tk.END, f"{filename}:\n")
            for test_name, status in tests:
                self.results_text.insert(tk.END, f"  {test_name}: {status}\n")
            self.results_text.insert(tk.END, "\n")

    def return_to_main(self):
        self.result_frame.pack_forget()
        self.main_frame.pack()

# Run the app
if __name__ == "__main__":
    root = tk.Tk()
    app = TestViewerApp(root)
    root.mainloop()
