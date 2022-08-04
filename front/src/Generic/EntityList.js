import { IconArrowDown, IconLoader } from "@tabler/icons";
import clsx from "clsx";
import { useNavigate } from "react-router-dom";
import EntityFieldView from "./EntityFieldView";
import { useState, useEffect } from "react";
import { refresh } from "../Users/slice";
import { useDispatch } from "react-redux";
import { IconArrowBigDown } from "@tabler/icons";


function filtered(fields) {
  if (!fields) {
    return fields;
  }
  return fields.filter((i) => i.type !== "key");
}

export default function EntityList({ loading, definition, entities, baseTo = null, selectedId }) {
  
  window.onscroll = () => {
    if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
      setPageNumber(pageNumber + 20)
    }
  };
  
  const navigate = useNavigate();
  const fields = filtered(definition?.fields);
  
  const [sortValue, setSortValue] = useState(0);
  const [sortKey, setSortKey] = useState("UserName");
  const dispatch = useDispatch();

  const [pageNumber, setPageNumber] = useState(20);

  useEffect(() => {
    dispatch(refresh(sortValue, sortKey));
    console.log("useEffect " + sortValue);
  }, [sortKey, sortValue])

  return (
    <>
    <div className="table-responsive">
    <button onClick={() => setSortValue(sortValue==1?-1:1)}>
      sorting
    </button>
      <table className="table card-table table-vcenter text-nowrap datatable table-hover">
        <thead>
          <tr>
            {fields &&
              fields.map((field, index) => (
                <th key={index} className={clsx({ "w-1": index === 0 })} >
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
    <button type="button" className="btn btn-icon btn-primary" onClick={() => setPageNumber(pageNumber + 20)}>
      <IconArrowBigDown />
    </button>
    </>
  );
}
