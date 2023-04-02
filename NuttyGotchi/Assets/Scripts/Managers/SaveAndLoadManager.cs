using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Utils;

namespace Managers {

	public class SaveAndLoadManager : Singleton<SaveAndLoadManager> {

		//si dichiara una stringa costante, che sarà il file dove verranno salvate le stats di Nutty
		private const string DATAPATH = "nuttyStats.NuttyGotchi";
		
		//si dichiara un oggetto di tipo NuttyStats che ci servirà quando ritorneremo le stats di Nutty dopo un loading
		private NuttyStats stats;

		//questo metodo ritorna il percorso del file
		private static string GetDataPath() {
			
			//e per farlo usiamo Path.Combine che ci permette di combinare due percorsi
			//il primo che andremo a prendere che rappresenta un diverso percorso in base alla piattaformo su cui andiamo a scrivere.
			//il secondo sarà il nome del file dichiarato prima
			return Path.Combine(Application.persistentDataPath, DATAPATH);
		}

		//questa funzione ci permetterà di sapere se il file è presente o no sul disco
		public bool NeedFirstSave() 
		{
			//se non è presente, salviamo.
			return !File.Exists(GetDataPath());
		}
		
		//con questa funzione rimuoviamo il file se presente sul disco
		public void RemoveSave() {
			File.Delete(GetDataPath());
		}
		
		//il primo salvataggio sarà differente dagli altri, perché abbiamo la necessità di crearlo e di ritornarlo
		public NuttyStats FirstSave() {
			 
			//creazione stats di Nutty
			if (NeedFirstSave()) {
				//dato che è la prima volta che le prendiamo possiamo porre tutte le stats a DataTime.Now
				//e attraverso il metodo di TimeUtils le convertiamo in ulong
				NuttyStats nuttyStats = new NuttyStats(
					TimeUtils.ULongFromDateTime(DateTime.Now),
					TimeUtils.ULongFromDateTime(DateTime.Now),
					TimeUtils.ULongFromDateTime(DateTime.Now),
					TimeUtils.ULongFromDateTime(DateTime.Now)
				);

				//per salvare si utilizza l'oggetto BinaryFormatter
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				
				//si specifica il percorso nel quale si vuole creare il file
				FileStream fileStream = File.Create(GetDataPath());
				
				//e utilizzare il metodo Serialize sul file appena creato
				//il primo è il file creato e il secondo la struttura dati creata poco prima da inserire
				binaryFormatter.Serialize(fileStream, nuttyStats);
				
				//importante da ricordarsi il .Close()
				fileStream.Close();

				return nuttyStats;
			}
			return null; 
		}
		
		//Creazione metodo dei salvataggi normali
		//passiamo come paramentro le stats da sovrascrivere
		public void Save(NuttyStats nuttyStats) {
			
			//sarà come la funzione di prima con l'unica differenza che non dovremmo creare una nuova struct di dati
			//ma utilizzeremo quella passataci come parametro
			if (!NeedFirstSave()) {
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				FileStream fileStream = File.Create(GetDataPath());
				binaryFormatter.Serialize(fileStream, nuttyStats);
				fileStream.Close();
			}
		}

		public NuttyStats Load() {
			if (!NeedFirstSave()) {
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				FileStream fileStream = File.Open(GetDataPath(), FileMode.Open);
				
				//prima del metodo serialize mettiamo le parentesi e la classe NuttyStats
				//questo rappresenta un cast, ovvero un cambio di tipo. Binary non ci tornerà direttamente una NuttyStats
				//ma noi avremmo bisogno di questo tipo di dato. Per cui in questo caso lo convertiamo.
				stats = (NuttyStats)binaryFormatter.Deserialize(fileStream);
				fileStream.Close();
				return stats;
			}
			return null; 
		}
	}
}
