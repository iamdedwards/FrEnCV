using	System;

namespace	PresentationV2
{
	public class FrenchNameAttribute : Attribute
	{
		public string  Name           { get; set; }

		public FrenchNameAttribute(string name)
		{
			Name = name;
		}
	}

	public	class Formation
	{
		[FrenchName("Depuis")]
		public int   From             { get; set; }
		[FrenchName("Jusqu'a")]
		public int   To               { get; set; }
		[FrenchName("Intitulé")]
		public string  Title          { get; set; }
		[FrenchName("Etablissment")]
		public string  Establishment  { get; set; }
	}

	public	class Experience
	{
		[FrenchName("Employeur")]
		public string       Employer  { get; set; }
		[FrenchName("Depuis")]
		public int          From      { get; set; }
		[FrenchName("Jusqu'a")]
		public int          To        { get; set; }
		[FrenchName("Poste")]
		public string       Position  { get; set; }
	}
}
