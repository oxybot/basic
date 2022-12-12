import { useParams, useRevalidator } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";

const transformDef = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "attachments");
  return d;
};

export function UserEdit({ full = false }) {
  const revalidator = useRevalidator();
  const { userId } = useParams();
  const definition = useDefinition("UserForEdit", transformDef);
  const [, entity] = useApiFetch(["Users", userId], { method: "GET" }, {});
  const texts = {
    title: entity.displayName,
    subTitle: "Edit a User",
  };

  function handleUpdate() {
    revalidator.revalidate();
  }

  return (
    <PageEdit
      definition={definition}
      entity={entity}
      texts={texts}
      full={full}
      baseApiUrl="Users"
      entityId={userId}
      onUpdate={handleUpdate}
    />
  );
}
