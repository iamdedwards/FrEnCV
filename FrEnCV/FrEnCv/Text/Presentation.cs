using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PresentationV1
{
	public class Formation
	{
	  public int			From                      { get; set; }
	  public int			To                        { get; set; }
	  public string			Title                     { get; set; }
	  public string			Establishment             { get; set; }
	}

	public class Experience
	{
	  public string       Employer                    { get; set; }
	  public int          From                        { get; set; }
	  public int          To                          { get; set; }
	  public string       Position                    { get; set; }
	}
}

