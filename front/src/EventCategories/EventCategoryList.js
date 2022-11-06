import { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageList from "../Generic/PageList";
import { eventCategoriesState, disconnect, setSorting, retrieveAll } from "./slice";

export function EventCategoryList() {
  const dispatch = useDispatch();
  const { categoryId } = useParams();
  const definition = useDefinition("EventCategoryForList");
  const texts = {
    title: "Event Categories",
    add: "Add category",
  };
  const { loading, elements, sorting } = useSelector(eventCategoriesState);

  useEffect(() => {
    dispatch(retrieveAll());
    return () => dispatch(disconnect());
  }, [dispatch, sorting]);

  return (
    <PageList
      definition={definition}
      loading={loading}
      elements={elements}
      selectedId={categoryId}
      texts={texts}
      newRole="time"
      sorting={sorting}
      setSorting={(s) => dispatch(setSorting(s))}
    />
  );
}
