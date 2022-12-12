import { useLoaderData, useParams } from "react-router-dom";
import { useDefinition } from "../api";
import { useSorting } from "../helpers/sorting";
import UserPageList from "../Users/UserPageList";

export function UserList() {
  const { userId } = useParams();
  const definition = useDefinition("UserForList");
  const texts = {
    title: "Users",
    add: "Add user",
    research: "Import user",
  };

  const elements = useLoaderData();
  const [sorting, updateSorting] = useSorting();

  return (
    <UserPageList
      definition={definition}
      elements={elements}
      selectedId={userId}
      texts={texts}
      newRole="user"
      sorting={sorting}
      setSorting={updateSorting}
    />
  );
}
