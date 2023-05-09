import { useLoaderData, useParams } from "react-router-dom";
import { useDefinition } from "../api";
import { useSorting } from "../helpers/sorting";
import PageList from "../Generic/PageList";

export function EventCategoryList() {
  const { categoryId } = useParams();
  const definition = useDefinition("EventCategoryForList");
  const texts = {
    title: "Event Categories",
    add: "Add category",
  };

  const elements = useLoaderData();
  const [sorting, updateSorting] = useSorting();

  return (
    <PageList
      definition={definition}
      elements={elements}
      selectedId={categoryId}
      texts={texts}
      newRole="schedules"
      sorting={sorting}
      setSorting={updateSorting}
    />
  );
}
