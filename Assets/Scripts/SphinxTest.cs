using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class SphinxTest : MonoBehaviour {
	string str;
	[SerializeField]
	GameObject cat;
	[SerializeField]
	GameObject dog;
	[SerializeField]
	GameObject human;
	[SerializeField]
	GameObject horse;
	[SerializeField]
	GameObject mouse;
	[SerializeField]
	GameObject monkey;
    [SerializeField]
    GameObject dick;
	[SerializeField]
	Transform spawn;

	// Use this for initialization
	void Start () {
		UnitySphinx.Init ();
		UnitySphinx.Run ();
        UnitySphinx.SetSearchModel(UnitySphinx.SearchModel.jsgf);
    }

	void Update()
	{
		str = UnitySphinx.DequeueString ();
        /*
		if (UnitySphinx.GetSearchModel() == "kws")
		{
			print ("listening for keyword");
			if (str != "") {
				UnitySphinx.SetSearchModel (UnitySphinx.SearchModel.jsgf);
				print (str);
			}
		}
		else if (UnitySphinx.GetSearchModel() == "jsgf")
		{
			print ("listening for order");
			if (str != "") 
			{
				char[] delimChars = { ' ' };
				string[] cmd = str.Split (delimChars);
				int numAnimals = interpretNum(cmd [0]);
				GameObject animal = interpretAnimal (cmd [1]);
				for (int i=0; i < numAnimals; i++) {
					Vector3 randPos = 
						new Vector3 (spawn.position.x + UnityEngine.Random.Range (-0.1f, 0.1f), 
							spawn.position.y + UnityEngine.Random.Range (-0.1f, 0.1f), 
							spawn.position.z + UnityEngine.Random.Range (-0.1f, 0.1f));
					Instantiate (animal, randPos, spawn.rotation);
				}
				UnitySphinx.SetSearchModel (UnitySphinx.SearchModel.kws);
			}
		}*/
        if (str != "")
        {
            char[] delimChars = { ' ' };
            string[] cmd = str.Split(delimChars);
            int numAnimals = interpretNum(cmd[0]);
            GameObject animal = interpretAnimal(cmd[1]);
            for (int i = 0; i < numAnimals; i++)
            {
                Vector3 randPos =
                    new Vector3(spawn.position.x + UnityEngine.Random.Range(-0.1f, 0.1f),
                        spawn.position.y + UnityEngine.Random.Range(-0.1f, 0.1f),
                        spawn.position.z + UnityEngine.Random.Range(-0.1f, 0.1f));
                Instantiate(animal, randPos, spawn.rotation);
            }
        }

    }

    GameObject interpretAnimal(string animal)
	{
		GameObject a = cat;
        if (animal == "cats")
            a = cat;
        else if (animal == "dogs" || animal == "bitches")
            a = dog;
        else if (animal == "horses")
            a = horse;
        else if (animal == "humans" || animal == "michael")
            a = human;
        else if (animal == "monkeys" || animal == "max")
            a = monkey;
        else if (animal == "mice" || animal == "fire")
            a = mouse;
        else if (animal == "dicks")
            a = dick;
		return a;
	}

	int interpretNum(string num)
	{
		int i = 0;
        if (num == "one")
            i += 1;
        else if (num == "two")
            i += 2;
        else if (num == "three")
            i += 3;
        else if (num == "four")
            i += 4;
        else if (num == "five")
            i += 5;
        else if (num == "six")
            i += 6;
        else if (num == "seven")
            i += 7;
        else if (num == "eight")
            i += 8;
        else if (num == "nine")
            i += 9;
        else if (num == "ten")
            i += 10;
		return i;
	}
}