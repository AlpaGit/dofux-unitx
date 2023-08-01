
namespace DofusCoube.FileProtocol.Datacenter.Misc
{
	public sealed class LuaFormula : IDofusObject
	{
		public static string Module => "LuaFormulas";

		public int Id { get; set; }

		public string FormulaName { get; set; }

		public string LuaFormula_ { get; set; }

	}
}
