import { useDispatch } from "react-redux";
import { apiFetch, useDefinition } from "../api";
import { setUser } from "../Authentication";
import PageEdit from "../Generic/PageEdit";

export function ProfileEdit({ full = false }) {
  const dispatch = useDispatch();
  const definition = useDefinition("MyUserForEdit");
  const texts = {
    title: (e) => e.displayName,
    subTitle: "Edit your Profile",
  };

  function handleUpdate() {
    apiFetch("My/User", { method: "GET" }).then((response) => dispatch(setUser(response)));
  }

  return <PageEdit definition={definition} texts={texts} full={full} baseApiUrl="My/User" onUpdate={handleUpdate} />;
}
