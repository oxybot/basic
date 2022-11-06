import { useDispatch } from "react-redux";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";
import AttachmentForm from "../Attachments/AttachmentForm";
import { retrieveAll } from "./slice";
import { useInRole } from "../Authentication";

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
  const isInRole = useInRole();

  function handleCreate() {
    dispatch(retrieveAll());
  }

  if (isInRole("beta")) {
    return (
      <PageNew
        definition={definition}
        baseApiUrl="Events/notify"
        texts={texts}
        extendedForm={(e, s, err) => <AttachmentForm entity={e} setEntity={s} errors={err} />}
        onCreate={handleCreate}
      />
    );
  } else {
    return <PageNew definition={definition} baseApiUrl="Events/notify" texts={texts} onCreate={handleCreate} />;
  }
}
