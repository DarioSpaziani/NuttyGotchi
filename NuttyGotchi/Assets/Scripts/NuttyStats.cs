using System;

[Serializable]
public class NuttyStats {
	public ulong birthDay;
	public ulong lastMeal;
	public ulong lastSleep;
	public ulong lastPlay;

	public NuttyStats(ulong birthDay, ulong lastMeal, ulong lastSleep, ulong lastPlay) {
		this.birthDay = birthDay; // con this ci riferiamo all'elemento della classe e quindi anche se 
		this.lastMeal = lastMeal; // nel costruttore avranno lo stesso nome potremmo disambiguare.
		this.lastSleep = lastSleep;
		this.lastPlay = lastPlay;
	}
}
