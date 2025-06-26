using Diary.Models.DiaryEntry;
using ZstdSharp.Unsafe;

namespace Diary.Services.DiaryEntry;

public interface IDiaryEntryService
{
    List<GetAllDiaryEntriesViewModel> GetAll();
    
    GetDiaryEntryViewModel GetById(int id);

    void Create(CreateEntryDto createEntryDto);

    void Update(int id, UpdateDiaryDto updateDiaryDto);

    void Delete(int id);
}