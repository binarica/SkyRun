namespace ScreenMgr {
	public interface IScreen {

		void Activate();
		void Deactivate();

		string Free();
	}
}