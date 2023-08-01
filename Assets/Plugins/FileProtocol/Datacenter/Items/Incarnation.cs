
namespace DofusCoube.FileProtocol.Datacenter.Items
{
	public sealed class Incarnation : IDofusObject
	{
		public static string Module => "Incarnation";

		public int Id { get; set; }

		public string LookMale { get; set; }

		public string LookFemale { get; set; }

		public int MaleBoneId { get; set; }

		public int FemaleBoneId { get; set; }

	}
}
