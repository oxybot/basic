import { useApiFetch, useDefinition } from "../api";
import PageEdit from "../Generic/PageEdit";

export function MyPasswordEdit({ full = false }) {
  const definition = useDefinition("PasswordForEdit");
  const [, entity] = useApiFetch("My/User", { method: "GET" }, {}); // a quoi ça sert ?
  const texts = {
    title: entity?.displayName,
    subTitle: "Edit your Password",
  };

  return (
    <PageEdit
      definition={definition}
      entity={entity}
      texts={texts}
      full={full}
      baseApiUrl="My/profile/password"
    />
  );
}