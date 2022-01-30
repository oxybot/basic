import { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { useParams } from "react-router-dom";
import { useDefinition } from "../api";
import PageList from "../Generic/PageList";
import { globalDaysOffState, disconnect, getAll } from "./slice";

export function GlobalDayOffList() {
  const dispatch = useDispatch();
  const { dayOffId } = useParams();
  const definition = useDefinition("GlobalDayOffForList");
  const texts = {
    title: "Global Days-Off",
    add: "Add day-off",
  };
  const { loading, values: elements } = useSelector(globalDaysOffState);

  useEffect(() => {
    dispatch(getAll());
    return () => dispatch(disconnect());
  }, [dispatch]);

  return (
    <PageList
      definition={definition}
      loading={loading}
      elements={elements}
      selectedId={dayOffId}
      texts={texts}
      newRole="time"
    />
  );
}
