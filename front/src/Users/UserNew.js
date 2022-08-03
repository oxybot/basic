import { useDispatch } from "react-redux";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";
import { refresh } from "./slice";
import AttachmentForm from "../Generic/AttachmentForm";

export function UserNew() {
  const dispatch = useDispatch();
  const definition = useDefinition("UserForEdit");
  const texts = {
    title: "Users",
    subTitle: "Add a new User",
  };

  function handleCreate() {
    dispatch(refresh());
  }

  return (
    <PageNew 
    definition={definition} 
    baseApiUrl="Users" 
    texts={texts} 
    onCreate={handleCreate} 
    extendedForm={(e, s, err) => <AttachmentForm entity={e} setEntity={s} errors={err} />}
    />
  );
}
