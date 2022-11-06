import { useDispatch } from "react-redux";
import { useDefinition } from "../api";
import PageNew from "../Generic/PageNew";
import { retrieveAll } from "./slice";

const transformDef = (d) => {
  d.fields = d.fields.filter((i) => i.name !== "attachments");
  return d;
};

export function UserNew() {
  const dispatch = useDispatch();
  const definition = useDefinition("UserForEdit", transformDef);
  const texts = {
    title: "Users",
    subTitle: "Add a new User",
  };

  function handleCreate() {
    dispatch(retrieveAll());
  }

  return <PageNew definition={definition} baseApiUrl="Users" texts={texts} onCreate={handleCreate} />;
}
