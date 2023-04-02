using System;

[Serializable]
public class NuttyStats {
	
	//use ulong because the most easy to convert from DateTime
	public ulong birthDay;
	public ulong lastMeal;
	public ulong lastSleep;
	public ulong lastPlay;

	//create a constructor for the struct data
	//this will accept 4 ulong values and will copy on our datas
	public NuttyStats(ulong birthDay, ulong lastMeal, ulong lastSleep, ulong lastPlay) {	
		this.birthDay = birthDay; // con this ci riferiamo all'elemento della classe e quindi anche se 
		this.lastMeal = lastMeal; // nel costruttore avranno lo stesso nome potremmo disambiguare.
		this.lastSleep = lastSleep;
		this.lastPlay = lastPlay;
	}
}
