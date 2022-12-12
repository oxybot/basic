import { useRevalidator } from "react-router-dom";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";

const transformDef = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "attachments");
  return d;
};

export function UserNew() {
  const revalidator = useRevalidator();
  const definition = useDefinition("UserForEdit", transformDef);
  const texts = {
    title: "Users",
    subTitle: "Add a new User",
  };

  function handleCreate() {
    revalidator.revalidate();
  }

  return <PageNew definition={definition} baseApiUrl="Users" texts={texts} onCreate={handleCreate} />;
}
