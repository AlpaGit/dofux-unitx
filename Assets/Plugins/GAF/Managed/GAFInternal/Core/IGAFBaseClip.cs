namespace GAF.Managed.GAFInternal.Core
{
	// Token: 0x0200008B RID: 139
	public interface IGAFBaseClip
	{
		/// <summary>
		/// Start playing animation.
		/// </summary>
		// Token: 0x06000440 RID: 1088
		void play();

		/// <summary>
		/// Check if animation is playing.
		/// </summary>
		/// <returns><c>true</c> if this instance is playing; otherwise, <c>false</c>.</returns>
		// Token: 0x06000441 RID: 1089
		bool isPlaying();

		/// <summary>
		/// Pause animation.
		/// </summary>
		// Token: 0x06000442 RID: 1090
		void pause();

		/// <summary>
		/// Stop playing animation. Moving playback to a first frame in a current sequence.
		/// </summary>
		// Token: 0x06000443 RID: 1091
		void stop();

		/// <summary>
		/// Go to the desired frame.
		/// </summary>
		/// <param name="_FrameNumber">Number of frame.</param>
		// Token: 0x06000444 RID: 1092
		void gotoAndStop(uint _FrameNumber);

		/// <summary>
		/// Go to the desired frame and start playing.
		/// </summary>
		/// <param name="_FrameNumber">Number of frame.</param>
		// Token: 0x06000445 RID: 1093
		void gotoAndPlay(uint _FrameNumber);

		/// <summary>
		/// Set sequence for playing.
		/// </summary>
		/// <param name="_SequenceName">Name of desired sequence.</param>
		/// <param name="_PlayImmediately">Run this sequence immediately.</param>
		// Token: 0x06000446 RID: 1094
		void setSequence(string _SequenceName, bool _PlayImmediately);

		/// <summary>
		/// Set playback to a default sequence.
		/// </summary>
		/// <param name="_PlayImmediately">Run this sequence immediately.</param>
		// Token: 0x06000447 RID: 1095
		void setDefaultSequence(bool _PlayImmediately);

		/// <summary>
		/// Get sequence name by index.
		/// </summary>
		/// <param name="_Index">Index of sequence.</param>
		/// <returns>System.String.</returns>
		// Token: 0x06000448 RID: 1096
		string sequenceIndexToName(uint _Index);

		/// <summary>
		/// Get sequence index by name.
		/// </summary>
		/// <param name="_Name">Name of sequence.</param>
		/// <returns>System.UInt32.</returns>
		// Token: 0x06000449 RID: 1097
		uint sequenceNameToIndex(string _Name);

		/// <summary>
		/// Get index of current sequence.
		/// </summary>
		/// <returns>System.UInt32.</returns>
		// Token: 0x0600044A RID: 1098
		uint getCurrentSequenceIndex();

		/// <summary>
		/// Get current frame number.
		/// </summary>
		/// <returns>System.UInt32.</returns>
		// Token: 0x0600044B RID: 1099
		uint getCurrentFrameNumber();

		/// <summary>
		/// Get count of frames in current timeline.
		/// </summary>
		/// <returns>System.UInt32.</returns>
		// Token: 0x0600044C RID: 1100
		uint getFramesCount();

		/// <summary>
		/// Get wrap mode.
		/// </summary>
		/// <returns>GAFWrapMode.</returns>
		// Token: 0x0600044D RID: 1101
		GAFWrapMode getAnimationWrapMode();

		/// <summary>
		/// Set wrap mode.
		/// </summary>
		/// <param name="_Mode">Type of wrap mode.</param>
		// Token: 0x0600044E RID: 1102
		void setAnimationWrapMode(GAFWrapMode _Mode);

		/// <summary>
		/// Get duration of current sequence.
		/// </summary>
		/// <returns>System.Single.</returns>
		// Token: 0x0600044F RID: 1103
		float duration();

		/// <summary>
		/// Remove event trigger by its ID.
		/// </summary>
		/// <param name="_ID">ID of trigger.</param>
		// Token: 0x06000450 RID: 1104
		void removeTrigger(int _ID);

		/// <summary>
		/// Remove all event triggers on selected frame.
		/// </summary>
		/// <param name="_FrameNumber">Number of frame.</param>
		// Token: 0x06000451 RID: 1105
		void removeAllTriggers(uint _FrameNumber);

		/// <summary>
		/// Remove all event triggers in current animation.
		/// </summary>
		// Token: 0x06000452 RID: 1106
		void removeAllTriggers();

		/// <summary>
		/// Get object name by it's ID.
		/// </summary>
		/// <param name="_ID">ID of object.</param>
		/// <returns></returns>
		// Token: 0x06000453 RID: 1107
		string objectIDToPartName(uint _ID);

		/// <summary>
		/// Get object id by name.
		/// </summary>
		/// <param name="_PartName">Named part name.</param>
		/// <returns></returns>
		// Token: 0x06000454 RID: 1108
		uint partNameToObjectID(string _PartName);
	}
}
