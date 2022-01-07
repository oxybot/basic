import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageList from "../Generic/PageList";
import { disconnect, getAll, schedulesState } from "./slice";

export function ScheduleList() {
  const dispatch = useDispatch();
  const { scheduleId } = useParams();
  const definition = useDefinition("ScheduleForList");
  const texts = {
    title: "Schedules",
    add: "Add schedule",
  };

  const { loading, values: elements } = useSelector(schedulesState);

  useEffect(() => {
    dispatch(getAll());
    return () => dispatch(disconnect());
  }, [dispatch]);

  return (
    <PageList
      definition={definition}
      loading={loading}
      elements={elements}
      selectedId={scheduleId}
      texts={texts}
      newRole="time"
    />
  );
}
