# TestViewerUI.py
import tkinter as tk
from tkinter import ttk, messagebox
from ItransferServerPC import get_all_users, download_zip, extract_results

class TestViewerApp:
    def __init__(self, master):
        self.master = master
        master.title("Test Viewer")

        self.user_list = []
        self.current_user_id = None

        self.setup_widgets()
        self.load_users()

    def setup_widgets(self):
        self.frame_select = ttk.Frame(self.master)
        self.frame_select.pack(pady=10)

        self.label = ttk.Label(self.frame_select, text="Choose User ID:")
        self.label.pack(side=tk.LEFT)

        self.user_combobox = ttk.Combobox(self.frame_select, state="readonly")
        self.user_combobox.pack(side=tk.LEFT, padx=5)

        self.select_button = ttk.Button(self.frame_select, text="View Results", command=self.select_user)
        self.select_button.pack(side=tk.LEFT)

        self.results_frame = ttk.Frame(self.master)
        self.results_text = tk.Text(self.results_frame, width=60, height=20)
        self.results_text.pack(padx=10, pady=10)

        self.back_button = ttk.Button(self.results_frame, text="Return", command=self.return_to_select)

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
        self.frame_select.pack_forget()
        self.results_frame.pack()
        self.back_button.pack(pady=(0,10))

        self.results_text.delete(1.0, tk.END)
        self.results_text.insert(tk.END, f"Results for User ID {user_id}:\n\n")

        for filename, tests in extracted_data.items():
            self.results_text.insert(tk.END, f"{filename}:\n")
            for test_name, status in tests:
                self.results_text.insert(tk.END, f"  {test_name}: {status}\n")
            self.results_text.insert(tk.END, "\n")

    def return_to_select(self):
        self.results_frame.pack_forget()
        self.frame_select.pack()

if __name__ == "__main__":
    root = tk.Tk()
    app = TestViewerApp(root)
    root.mainloop()
