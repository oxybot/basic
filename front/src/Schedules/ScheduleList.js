import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageList from "../Generic/PageList";
import { disconnect, retrieveAll, schedulesState, setSorting } from "./slice";

export function ScheduleList() {
  const dispatch = useDispatch();
  const { scheduleId } = useParams();
  const definition = useDefinition("ScheduleForList");
  const texts = {
    title: "Schedules",
    add: "Add schedule",
  };

  const { loading, elements, sorting } = useSelector(schedulesState);

  useEffect(() => {
    dispatch(retrieveAll());
    return () => dispatch(disconnect());
  }, [dispatch, sorting]);

  return (
    <PageList
      definition={definition}
      loading={loading}
      elements={elements}
      selectedId={scheduleId}
      texts={texts}
      newRole="time"
      sorting={sorting}
      setSorting={(s) => dispatch(setSorting(s))}
    />
  );
}
