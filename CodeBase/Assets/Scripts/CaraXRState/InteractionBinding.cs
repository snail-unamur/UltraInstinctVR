using UnityEngine;

public class InteractionBinding
{


    private Modalities[] modalities;
    private Object actionToBeEffected;


    // General methods for modalities
    public virtual void Using(Modalities[] modalities)
    {
        this.modalities = modalities;

    }

    public virtual void Interaction()
    {

    }


    public virtual void ToProduce(Object actionToBeEffected)
    {
        this.actionToBeEffected = actionToBeEffected;
    }



    //  Method for interactions

    public object initializeInteraction(Modalities modalityOne, Modalities modalityTwo)
    {
        // Initialize interaction logic here
        return null;
    }


    public object doInteraction(Modalities modalityOne, Modalities modalityTwo)
    {
        // Perform interaction logic here
        return null;
    }


    public object combineInteractions(Modalities modalityOne, Modalities modalityTwo)
    {
        // Combine interactions logic here
        return null;
    }


}
