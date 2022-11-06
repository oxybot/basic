import { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import { usersState, disconnect, retrieveAll, setSorting } from "./slice";
import UserPageList from "../Users/UserPageList";

export function UserList() {
  const dispatch = useDispatch();
  const { userId } = useParams();
  const definition = useDefinition("UserForList");
  const texts = {
    title: "Users",
    add: "Add user",
    research: "Import user",
  };

  const { loading, elements, sorting } = useSelector(usersState);

  useEffect(() => {
    dispatch(retrieveAll());
    return () => dispatch(disconnect());
  }, [dispatch, sorting]);

  return (
    <UserPageList
      definition={definition}
      loading={loading}
      elements={elements}
      selectedId={userId}
      texts={texts}
      newRole="user"
      sorting={sorting}
      setSorting={(s) => dispatch(setSorting(s))}
    />
  );
}
