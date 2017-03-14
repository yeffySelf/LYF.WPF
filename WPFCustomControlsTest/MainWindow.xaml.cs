using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WPFCustomControls;
using WPFCustomControlsTest.ViewModel;

namespace WPFCustomControlsTest
{
    public class DepartmentModel
    {
        public List<DepartmentModel> Nodes { get; set; }
        public DepartmentModel()
        {
            this.Nodes = new List<DepartmentModel>();
            this.ParentId = 0;//主节点的父id默认为0
        }
        public int id { get; set; }//id
        public string deptName { get; set; }//部门名称
        public int ParentId { get; set; }//父类id
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            singleBindItem.ItemsSource = getTrees(0, getDepts());
            multipleBindItem.ItemsSource = getTrees(0, getDepts());
        }

        public List<DepartmentModel> getTrees(int parentid, List<DepartmentModel> nodes)
        {
            List<DepartmentModel> mainNodes = nodes.Where(x => x.ParentId == parentid).ToList<DepartmentModel>();
            List<DepartmentModel> otherNodes = nodes.Where(x => x.ParentId != parentid).ToList<DepartmentModel>();
            foreach (DepartmentModel dpt in mainNodes)
            {
                dpt.Nodes = getTrees(dpt.id, otherNodes);
            }
            return mainNodes;
        }

        public List<DepartmentModel> getDepts()
        {
            List<DepartmentModel> dplst = new List<DepartmentModel>(){
            new DepartmentModel(){id=1,deptName="主部门1",ParentId=0},
            new DepartmentModel(){id=2,deptName="主部门2",ParentId=0},
            new DepartmentModel(){id=3,deptName="主部门1_1",ParentId=1},
            new DepartmentModel(){id=4,deptName="主部门1_2",ParentId=1},
            new DepartmentModel(){id=5,deptName="主部门1_3",ParentId=1},
            new DepartmentModel(){id=6,deptName="主部门1_4",ParentId=1},
            new DepartmentModel(){id=7,deptName="主部门1_5",ParentId=1},
            new DepartmentModel(){id=8,deptName="主部门2_1",ParentId=2},
            new DepartmentModel(){id=9,deptName="主部门2_2",ParentId=2},
            new DepartmentModel(){id=10,deptName="主部门2_3",ParentId=2},
            new DepartmentModel(){id=11,deptName="主部门2_4",ParentId=2},
            new DepartmentModel(){id=12,deptName="主部门1_1_1",ParentId=3},
            new DepartmentModel(){id=13,deptName="主部门1_1_2",ParentId=3},
            new DepartmentModel(){id=14,deptName="主部门1_2_1",ParentId=4},
            new DepartmentModel(){id=15,deptName="主部门1_1_1_1",ParentId=12}
            };
            return dplst;
        }

        private void CustomTreeView_SelectedItemsChanged(object sender, RoutedEventArgs e)
        {
            CustomTreeView tv = sender as CustomTreeView;
            label.Text = string.Empty;
            tv.SelectedItems.ForEach(r=>
            {
                CustomTreeViewItem item = r as CustomTreeViewItem;
                if(item!=null)
                label.Text += item.Header + ";";
            });
        }
    }
}