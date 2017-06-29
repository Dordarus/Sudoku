using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public interface IFileWorker
    {
        Task<bool> ExistsAsync(string filename);
        Task SaveTextAsync(string filename, string text);
        Task<string> LoadTextAsync(string filename);
        Task<IEnumerable<string>> GetFilesAsync();
        Task DeleteAsync(string filename);
    }
}
