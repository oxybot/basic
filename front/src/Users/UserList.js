import { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import { usersState, disconnect, getAll } from "./slice";
import PageList from "../Generic/PageList";

export function UserList() {
  const dispatch = useDispatch();
  const { userId } = useParams();
  const definition = useDefinition("UserForList");
  const texts = {
    title: "Users",
    add: "Add user",
  };
  const { loading, values: elements } = useSelector(usersState);

  useEffect(() => {
    dispatch(getAll());
    return () => dispatch(disconnect());
  }, [dispatch]);

  return (
    <PageList
      definition={definition}
      loading={loading}
      elements={elements}
      selectedId={userId}
      texts={texts}
      newRole="user"
      />
  );
}
