using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ArrayExtensionMethods // : MonoBehaviour
{
    ///<summary>
    ///Adds an element to array at index 0.
    ///</summary>
    public static Array AddToArray(this Array a, object o)
    {
        if (a.GetType().GetElementType() == o.GetType() || o.GetType().IsSubclassOf(a.GetType().GetElementType()))
        {
            Array b = Array.CreateInstance(a.GetType().GetElementType(), a.Length + 1);
            a.CopyTo(b, 1);
            b.SetValue(o, 0);

            a = b;
        }
        else
        {
            Debug.Log("Type mismatch, object not added. -- (Type) "
                + a.GetType().GetElementType() + " != (Type) " + o.GetType());
        }
        return a;
    }
    ///<summary>
    ///Adds an element at the given index.
    ///</summary>
    public static Array AddToArrayAtIndex(this Array a, object o, int index)
    {
        if (index > a.Length)
        {
            Debug.Log("Index outside the bounds of array. Object not added.");
        }
        else
        {
            if (a.GetType().GetElementType() == o.GetType() || o.GetType().IsSubclassOf(a.GetType().GetElementType()))
            {
                Array b = Array.CreateInstance(a.GetType().GetElementType(), a.Length + 1);

                for (int i = 0; i < index; ++i)
                {
                    b.SetValue(a.GetValue(i), i);
                }
                for (int i = index + 1; i < b.Length; ++i)
                {
                    b.SetValue(a.GetValue(i - 1), i);
                }

                b.SetValue(o, index);
                a = b;
            }
            else
            {
                Debug.Log("Type mismatch, object not added. -- (Type) "
                    + a.GetType().GetElementType() + " != (Type) " + o.GetType());
            }
        }
        return a;
    }
    ///<summary>
    ///Removes the first incident of a given element in an array.
    ///</summary>
    public static Array Remove(this Array a, object o)
    {
        if (a.GetType().GetElementType() == o.GetType() || o.GetType().IsSubclassOf(a.GetType().GetElementType()))
        {
            if (a.Length==0)
            {
               // Debug.Log("array length already 0. Removal failed.");
                return a;
            }
            int occurrences = 0;
            for (int i=0;i<a.Length;++i)
            {
                if (a.GetValue(i).Equals(o))
                {
                    occurrences++;
                }
            }
            Array b = Array.CreateInstance(a.GetType().GetElementType(), a.Length - occurrences);
            int index = 0;
            for (int i = 0; i < a.Length; ++i)
            {
                if (!a.GetValue(i).Equals(o))
                {
                    b.SetValue(a.GetValue(i), index);
                    index++;

                }
            }
            a = b;
        }
        else
        {
            Debug.Log("Mismatched type passed as parameter for Array.Remove() -- (Type) "
                + a.GetType().GetElementType() + " != (Type) " + o.GetType());
        }
        return a;
    }
    ///<summary>
    ///Removes the element with a given index from an array.
    ///</summary>
    public static Array RemoveFromArrayAtIndex(this Array a, int index)
    {
        if (index >= a.Length)
        {
            Debug.Log("Index outside the bounds of the array. Object not removed.");
        }
        else
        {
            if (a.Length > 0)
            {
                Array b = Array.CreateInstance(a.GetType().GetElementType(), a.Length - 1);
                for (int i = 0; i < index; ++i)
                {
                    b.SetValue(a.GetValue(i), i);
                }
                for (int i = index; i < b.Length; ++i)
                {
                    b.SetValue(a.GetValue(i + 1), i);
                }

                a = b;
            }
            else
            {
               // Debug.Log("array has no elements to remove");
            }
        }
        return a;
    }
    /// <summary>
    /// shuffles using the Fisher-Yates method
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static Array EfficientShuffle(this Array a)
    {
        //uses the Fisher-Yates method of shuffling
        Array b = Array.CreateInstance(a.GetType().GetElementType(), a.Length);
        var randomness = new System.Random();

        for (int i = a.Length - 1; i >= 0; --i)
        {
            int rand = randomness.Next(a.Length);
            b.SetValue(a.GetValue(rand), i);
            a = a.RemoveFromArrayAtIndex(rand);
        }
        return b;
    }
    ///<summary>
    ///Shuffles an array using a random number of swaps.
    ///</summary>
    public static Array Shuffle(this Array a)
    {
        int replacements = UnityEngine.Random.Range(100, 1000);

        for (int i = 0; i < replacements; i++)
        {

            int A = UnityEngine.Random.Range(0, a.Length);
            int B = UnityEngine.Random.Range(0, a.Length);

             
            object e = a.GetValue(A);
            object f = a.GetValue(B);
            object g = a.GetValue(A);

            e = f;
            f = g;

            a.SetValue(e,A);
            a.SetValue(f, B);
        }
        
        return a;
    }


}
