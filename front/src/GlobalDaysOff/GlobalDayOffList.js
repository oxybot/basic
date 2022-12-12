import { useLoaderData, useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageList from "../Generic/PageList";
import { useSorting } from "../helpers/sorting";

export function GlobalDayOffList() {
  const { dayOffId } = useParams();
  const definition = useDefinition("GlobalDayOffForList");
  const texts = {
    title: "Global Days-Off",
    add: "Add day-off",
  };

  const elements = useLoaderData();
  const [sorting, updateSorting] = useSorting();

  return (
    <PageList
      definition={definition}
      elements={elements}
      selectedId={dayOffId}
      texts={texts}
      newRole="time"
      sorting={sorting}
      setSorting={updateSorting}
    />
  );
}
