using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FrEnCV
{
	public class ResourceController : NotifyPropertyChanged
	{
		private ResourceController() { }

		public static ResourceController					_instance;
		public static ResourceController					Instance	{ get { return _instance = _instance ?? new ResourceController();  } }
		public  FrEn<ObservableCollection<CollectionView>>  Data		{ get; private set; }
		public	string										DataName	{ get { return dataNames[DataIndex]; } }

		private string[] _dataNames;
		private string[] dataNames
		{
			get
			{
				return _dataNames = _dataNames ?? new string[] 
				{
					"Presentation",
					"Experience"
				};

			}
		}
		private	int				_dataIndex;
        public int				DataIndex
		{
			get { return _dataIndex;}
			set
			{
				if (value >= dataNames.Count())
					_dataIndex = 0;
				else
					_dataIndex = value;
				
				OnPropertyChanged(nameof(DataName));
			}
		}

		public bool		DataEnd
		{ 
			get
			{
				return (DataIndex + 1 >= dataNames.Count());
			}
		}

		public void				NextDataItem(MainWindow win)
		{
			DataIndex += 1;
			win.Stdout = null;
			win.Term.Display.IsEnabled = true;
			win.StdOutBlock.Text = "";
		}

		public void SetResources(CV cv)
		{
			Data = new FrEn<ObservableCollection<CollectionView>>();
			Data.Fr = new ObservableCollection<CollectionView>(){
				CollectionView.For("Fr", cv.Experiences.Fr),
				CollectionView.For("Fr", cv.Formations.Fr),
			};
			Data.En = new ObservableCollection<CollectionView>(){
				CollectionView.For("En", cv.Experiences.En),
				CollectionView.For("En", cv.Formations.En),
			};
		}

		public CollectionView GetData<T>()
		{
			if (typeof(T).Name == typeof(Formation).Name)
				return (Data.Current().ElementAt(0));
			if (typeof(T).Name == typeof(Experience).Name)
				return (Data.Current().ElementAt(1));
			throw new ArgumentException($"Unsupported Type in Method GetData : {typeof(T)}");
		}
	}
}
