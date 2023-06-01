namespace WondeluxeEditor
{
	internal struct SerializedPropertyGroupInfo
	{
		public string Name;
		public int Order;
		public int StartIndex;
		public bool Foldout;

		public SerializedPropertyGroupInfo(string name, int order, int startIndex, bool foldout)
		{
			Name = name;
			Order = order;
			StartIndex = startIndex;
			Foldout = foldout;
		}

		public override string ToString()
		{
			return $"(Name = {Name}, Order = {Order}, StartIndex = {StartIndex}, Foldout = {Foldout})";
		}
	}
}
