import { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageList from "../Generic/PageList";
import { eventCategoriesState, disconnect, getAll } from "./slice";

export function EventCategoryList() {
  const dispatch = useDispatch();
  const { categoryId } = useParams();
  const definition = useDefinition("EventCategoryForList");
  const texts = {
    title: "Event Categories",
    add: "Add category",
  };
  const { loading, values: elements } = useSelector(eventCategoriesState);

  useEffect(() => {
    dispatch(getAll());
    return () => dispatch(disconnect());
  }, [dispatch]);

  return (
    <PageList definition={definition} loading={loading} elements={elements} selectedId={categoryId} texts={texts} />
  );
}
