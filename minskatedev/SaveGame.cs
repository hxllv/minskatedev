using System;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace minskatedev
{
    public partial class Game
    {
        XmlSerializer serializer;
        int deck, trucks, wheels;


        [Serializable]
        public struct Sk8
        {
            public int deck, trucks, wheels;

            public Sk8(int deck, int trucks, int wheels)
            {
                this.deck = deck;
                this.trucks = trucks;
                this.wheels = wheels;
            }
        }

        //public SaveLoadManager()
        //{
        //    serializer = new XmlSerializer(typeof(Sk8));
        //}

        public void SaveSk8(int deckInd, int trucksInd, int wheelInd)
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
            IsolatedStorageFileStream isoStoreStream = null;
            serializer = new XmlSerializer(typeof(Sk8));

            Sk8 sk8 = new Sk8(deckInd, trucksInd, wheelInd);

            //Save it
            if (isoStore.FileExists("conf.sk8"))
            {
                System.Diagnostics.Debug.WriteLine(isoStore.FileExists("conf.sk8"));
                isoStore.DeleteFile("conf.sk8");
            }
            using (isoStoreStream = isoStore.CreateFile("conf.sk8"))
            {
                System.Diagnostics.Debug.WriteLine("writin");
                // Set the position to the begining of the file.
                isoStoreStream.Seek(0, System.IO.SeekOrigin.Begin);
                // Serialize the new data object.
                serializer.Serialize(isoStoreStream, sk8);
                // Set the length of the file.
                isoStoreStream.SetLength(isoStoreStream.Position);

            }

            isoStore.Close();
            isoStoreStream.Dispose();
        }

        public void LoadSk8()
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
            IsolatedStorageFileStream isoStoreStream = null;
            serializer = new XmlSerializer(typeof(Sk8));

            if (isoStore.FileExists("conf.sk8"))
            {
                // Open the file using the established file stream.
                using (isoStoreStream = isoStore.OpenFile("conf.sk8", System.IO.FileMode.Open))
                {
                    // Store the deserialized data object.
                    Sk8 sk8 = (Sk8)serializer.Deserialize(isoStoreStream);

                    //Extract the save data
                    menu.deckInd = sk8.deck;
                    menu.truckInd = sk8.trucks;
                    menu.wheelInd = sk8.wheels;

                    //Loop through SaveData.ownedSoccerBalls and use the wrapper data to recreate the player's ownedSoccerBalls
                }
                isoStore.Close();
                isoStoreStream.Close();
            }
            else
            {
                menu.deckInd = -1;
                menu.truckInd = -1;
                menu.wheelInd = -1;
            }
        }
    }
}
