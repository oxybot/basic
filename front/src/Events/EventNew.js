import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";
import AttachmentForm from "../Attachments/AttachmentForm";
import { useInRole } from "../Authentication";
import { useRevalidator } from "react-router-dom";

const transform = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "attachments");
  return d;
};

export function EventNew() {
  const revalidator = useRevalidator();
  const definition = useDefinition("EventForEdit", transform);
  const texts = {
    title: "Events",
    subTitle: "Add a new Event",
    "form-action": "Create",
  };
  const isInRole = useInRole();

  function handleCreate() {
    revalidator.revalidate();
  }

  if (isInRole("beta")) {
    return (
      <PageNew
        definition={definition}
        baseApiUrl="Events"
        texts={texts}
        extendedForm={(e, s, err) => <AttachmentForm entity={e} setEntity={s} errors={err} />}
        onCreate={handleCreate}
      />
    );
  } else {
    return <PageNew definition={definition} baseApiUrl="Events" texts={texts} onCreate={handleCreate} />;
  }
}
