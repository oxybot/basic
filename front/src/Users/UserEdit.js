import { useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useApiFetch, useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";
import { refresh } from "./slice";
import AttachmentForm from "../Attachments/AttachmentForm";

const transformDef = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "attachments");
  return d;
};

export function UserEdit({ full = false }) {
  const dispatch = useDispatch();
  const { userId } = useParams();
  const definition = useDefinition("UserForEdit", transformDef);
  const [, entity] = useApiFetch(["Users", userId], { method: "GET" }, {});
  const texts = {
    title: entity.displayName,
    subTitle: "Edit a User",
  };

  function handleUpdate() {
    dispatch(refresh());
  }

  return (
    <PageEdit
      definition={definition}
      entity={entity}
      texts={texts}
      full={full}
      baseApiUrl="Users"
      entityId={userId}
      extendedForm={(e, s, err) => <AttachmentForm entity={e} setEntity={s} errors={err} />}
      onUpdate={handleUpdate}
    />
  );
}
