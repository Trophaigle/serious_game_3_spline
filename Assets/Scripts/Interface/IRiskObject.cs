using UnityEngine;

public interface IRiskObject: IHoverable
{
   
   void OnCorrectlyIdentified();
    void OnWronglyIdentified();
}
