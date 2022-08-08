import { IconArrowBigDown, IconLoader } from "@tabler/icons";
import clsx from "clsx";
import { useNavigate } from "react-router-dom";
import EntityFieldView from "./EntityFieldView";
import { useState, useEffect } from "react";
import { refresh as refreshUsers} from "../Users/slice";
import { refresh as refreshEvents } from "../Events/slice";
import { refresh as refreshBalances } from "../Balances/slice";
import { refresh as refreshSchedules } from "../Schedules/slice";
import { useDispatch } from "react-redux";

// ICI filter a supprimer

function filtered(fields, filter) {
  if (!fields) {
    return fields;
  }
  return fields.filter((i) => i.type !== "key");
}

export default function EntityList({ loading, definition, entities, baseTo = null, selectedId, filter }) {
  
  window.onscroll = () => {
    if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
      setPageNumber(pageNumber + 20)
    }
  };
  
  const navigate = useNavigate();
  const fields = filtered(definition?.fields, filter);
  
  const [sortValue, setSortValue] = useState("noSort");
  const [sortKey, setSortKey] = useState("UserName");
  const dispatch = useDispatch();
  const numberOfRowToDisplay = 24;
  const [pageNumber, setPageNumber] = useState(numberOfRowToDisplay);

  useEffect(() => {
    dispatch(refreshUsers(sortValue, sortKey, null));
    dispatch(refreshEvents(sortValue, sortKey, null));
    dispatch(refreshBalances(sortValue, sortKey, null));
    dispatch(refreshSchedules(sortValue, sortKey, null));
  }, [sortKey, sortValue])

  return (
    <>
    <div className="table-responsive">
      <table className="table card-table table-vcenter text-nowrap datatable table-hover">
        <thead>
          <tr>
            {fields &&
              fields.map((field, index) => (
                <th key={index} className={"sorting " + clsx({ "w-1": index === 0 })} onClick={() => {
                  setSortValue(sortValue=="asc"?"desc":"asc"); 
                  setSortKey(field.displayName);
                  }}>
                  {field.displayName}
                </th>
              ))}
          </tr>
        </thead>
        <tbody>
          <tr className={loading ? "" : "d-none"}>
            <td colSpan={fields?.length}>
              <IconLoader /> Loading...
            </td>
          </tr>
          {entities.slice(0, pageNumber).map((entity) => (
            <tr
            key={entity.identifier}
            className={clsx({
              "table-active": entity.identifier === selectedId,
            })}
            onClick={() => baseTo !== null && navigate([baseTo, entity.identifier].filter((i) => i).join("/"))}
            >
              {entity &&
                fields &&
                fields.map((field, index) => (
                  <td key={index}>
                    <EntityFieldView type={field.type} value={entity[field.name]} list />
                  </td>
                ))}
            </tr>
          ))}
          {!loading && entities.length === 0 && (
            <tr>
              <td colSpan={fields?.length}>
                <em>No results</em>
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
    </>
  );
}
