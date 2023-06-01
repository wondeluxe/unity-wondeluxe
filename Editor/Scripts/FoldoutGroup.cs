namespace WondeluxeEditor
{
	internal class FoldoutGroup
	{
		public string Name { get; private set; }
		public bool Expanded { get; set; }

		public FoldoutGroup(string name, bool expanded = true)
		{
			Name = name;
			Expanded = expanded;
		}
	}
}