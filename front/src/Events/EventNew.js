import { useDispatch } from "react-redux";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";
import AttachmentForm from "../Generic/AttachmentForm";
import { refresh } from "./slice";

const transform = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "attachments");
  return d;
};

export function EventNew() {
  const dispatch = useDispatch();
  const definition = useDefinition("EventForEdit", transform);
  const texts = {
    title: "Events",
    subTitle: "Add a new Event",
    "form-action": "Create",
  };

  function handleCreate() {
    dispatch(refresh());
  }

  return (
    <PageNew 
    definition={definition} 
    baseApiUrl="Events/notify"
    texts={texts}
    extendedForm={(e, s, err) => <AttachmentForm entity={e} setEntity={s} errors={err} />}
    onCreate={handleCreate} 
    />
  );
}
