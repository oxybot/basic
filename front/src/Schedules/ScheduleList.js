import { useLoaderData, useParams } from "react-router-dom";
import { useDefinition } from "../api";
import { useSorting } from "../helpers/sorting";
import PageList from "../Generic/PageList";

export function ScheduleList() {
  const { scheduleId } = useParams();
  const definition = useDefinition("ScheduleForList");
  const texts = {
    title: "Schedules",
    add: "Add schedule",
  };

  const elements = useLoaderData();
  const [sorting, updateSorting] = useSorting();

  return (
    <PageList
      definition={definition}
      elements={elements}
      selectedId={scheduleId}
      texts={texts}
      newRole="schedules"
      sorting={sorting}
      setSorting={updateSorting}
    />
  );
}
