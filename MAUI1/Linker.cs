using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI1
{
    internal static class Linker
    {
        public static ObservableCollection<object> ViewModels { get; set; } = new ObservableCollection<object>();
        static Linker()
        {
            ViewModels.CollectionChanged += ViewModels_CollectionChanged;
        }

        private static void ViewModels_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            ObservableCollection<object>? vm;
            if (sender is not null)
            {
                vm = sender as ObservableCollection<object>;
            }
            else
            {
                return;
            }
            for (int i = 0; i < ViewModels.Count; i++)
            {
                for (int j = 0; j < vm.Count - 1; j++)
                {
                    if (vm[i].GetType().Name == ViewModels[j].GetType().Name)
                    {
                        if (ViewModels.Where(item => item.GetType().Name == vm[i].GetType().Name).Count() > 1)
                        {
                            ViewModels.RemoveAt(j);
                        }
                    }
                }
            }
        }
    }
}
