using HutongGames.PlayMaker;
namespace com.CanadianTire.playmaker.actions {

    public class FindChildWithTag : FsmStateAction {

        [Tooltip("The collided with Game Object"), RequiredField]
        public FsmOwnerDefault Parent;

        [RequiredField]
        public FsmString Tag;

        [UIHint(UIHint.Variable), RequiredField]
        public FsmGameObject StoreValue;

        public override void OnEnter()
        {
            StoreValue.Value = Fsm.GetOwnerDefaultTarget(Parent).transform.GetChild(0).FindChildByTag(Tag.Value).gameObject;
            Finish();
        }
    }
}
