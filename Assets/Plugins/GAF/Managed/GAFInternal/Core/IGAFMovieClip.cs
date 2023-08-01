using GAF.Managed.GAFInternal.Objects;

namespace GAF.Managed.GAFInternal.Core
{
	// Token: 0x0200008C RID: 140
	public interface IGAFMovieClip : IGAFBaseClip
	{
		/// <summary>
		/// Getting reference to an object by ID
		/// </summary>
		/// <param name="_ID">ID of animation suboject e.g "2_27" - "27" is object ID.</param>
		/// <returns>IGAFObject.</returns>
		// Token: 0x06000455 RID: 1109
		IGAFObject getObject(uint _ID);

		/// <summary>
		/// Get object by name. If your object has custom name in inspector you can get it here.
		/// </summary>
		/// <param name="_PartName">Name of part.</param>
		/// <returns></returns>
		// Token: 0x06000456 RID: 1110
		IGAFObject getObject(string _PartName);
	}
}
