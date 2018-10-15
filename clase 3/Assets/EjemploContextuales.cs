using UnityEngine;
using UnityEditor;

public class EjemploContextuales : MonoBehaviour
{
    public static string _myName;
	//Agrega un context a las variables en si.
	//Primer parmetro: Nombre de la opcion
	//Segundo parametro: Funcion que funciona como callback
	[ContextMenuItem("RandomizeName", "Randomize")]
	public int myHP;
	
	//con este método podemos AGREGAR items al menu ya por defecto
	//El MenuCommand se puede agregar para conseguir referencia al componente que clikeé
	[MenuItem("CONTEXT/Transform/MyCustomReset")]
	private static void MyTransformOption(MenuCommand menuCommand)
	{
		var tr = menuCommand.context as Transform;
		tr.position = Vector3.one;
		tr.rotation = Quaternion.identity;
	}
	
	private void Randomize()
	{
		myHP = Random.Range(0,101);
	}
}
