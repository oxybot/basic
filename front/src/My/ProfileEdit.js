import { useDispatch } from "react-redux";
import { apiFetch, useApiFetch, useDefinition } from "../api";
import { setUser } from "../Authentication";
import PageEdit from "../Generic/PageEdit";

export function ProfileEdit({ full = false }) {
  const dispatch = useDispatch();
  const definition = useDefinition("MyUserForEdit");
  const [, entity] = useApiFetch("My/User", { method: "GET" }, {});
  const texts = {
    title: entity?.displayName,
    subTitle: "Edit your Profile",
  };

  function handleUpdate() {
    apiFetch("My/User", { method: "GET" }).then((response) => dispatch(setUser(response)));
  }

  return (
    <PageEdit
      definition={definition}
      entity={entity}
      texts={texts}
      full={full}
      baseApiUrl="My/User"
      onUpdate={handleUpdate}
    />
  );
}
