using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HospitalBaseApp.Services
{
    public class PageService
    {
        private Stack<Page> _history;
        private Page _lastPage;
        public event Action<Page> OnPageChanged;

        public bool CanGoToBack => _history.Skip(1).Any();
        public void ChangePage(Page page)
        {
             OnPageChanged?.Invoke(page);
            

           
                _history.Push(page);
            
        }

        public PageService()
        {
            _history = new Stack<Page>();
        }

        public void GoToBack()
        {
             _history.Pop();
            var page = _history.Peek();
            OnPageChanged?.Invoke(page);
        }
    }
}
