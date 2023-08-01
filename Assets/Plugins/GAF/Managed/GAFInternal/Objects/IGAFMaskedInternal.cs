namespace GAF.Managed.GAFInternal.Objects
{
	// Token: 0x0200004D RID: 77
	internal interface IGAFMaskedInternal
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001C0 RID: 448
		uint objectID { get; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001C1 RID: 449
		float zPosition { get; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001C2 RID: 450
		bool isVisible { get; }

		// Token: 0x060001C3 RID: 451
		void enableMasking();

		// Token: 0x060001C4 RID: 452
		void updateMasking();

		// Token: 0x060001C5 RID: 453
		void disableMasking();
	}
}
