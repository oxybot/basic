import { IconSearch } from "@tabler/icons";
import { useState } from "react";
import { useLoaderData, useParams } from "react-router-dom";
import { useDefinition } from "../api";
import { useSorting } from "../helpers/sorting";
import { useFiltering } from "../helpers/filtering";
import PageList from "../Generic/PageList";
import "./card.scss";

function EventFilters({ search, onSearchChange, statuses, onStatusChange }) {
  return (
    <>
      <label className="form-label">Search</label>
      <div className="input-icon mb-3">
        <input type="text" className="form-control" placeholder="Search…" value={search} onChange={onSearchChange} />
        <span className="input-icon-addon">
          <IconSearch />
        </span>
      </div>
      <label className="form-label">Status</label>
      <div className="form-selectgroup">
        <label className="form-selectgroup-item">
          <input
            type="checkbox"
            name="status"
            value="requested"
            className="form-selectgroup-input"
            checked={statuses.requested || false}
            onChange={onStatusChange}
          />
          <span className="form-selectgroup-label">Requested</span>
        </label>
        <label className="form-selectgroup-item">
          <input
            type="checkbox"
            name="status"
            value="approved"
            className="form-selectgroup-input"
            checked={statuses.approved || false}
            onChange={onStatusChange}
          />
          <span className="form-selectgroup-label">Approved</span>
        </label>
        <label className="form-selectgroup-item">
          <input
            type="checkbox"
            name="status"
            value="rejected"
            className="form-selectgroup-input"
            checked={statuses.rejected || false}
            onChange={onStatusChange}
          />
          <span className="form-selectgroup-label">Rejected</span>
        </label>
        <label className="form-selectgroup-item">
          <input
            type="checkbox"
            name="status"
            value="canceled"
            className="form-selectgroup-input"
            checked={statuses.canceled || false}
            onChange={onStatusChange}
          />
          <span className="form-selectgroup-label">Canceled</span>
        </label>
      </div>
    </>
  );
}

export function EventList() {
  const { eventId } = useParams();
  const definition = useDefinition("EventForList");
  const texts = {
    title: "Events",
    add: "Add event",
  };

  const elements = useLoaderData();
  const [sorting, updateSorting] = useSorting();
  const [filters, setFilters] = useFiltering();

  const [search, setSearch] = useState("");

  function handleStatusChange(event) {
    let target = event.target;
    setFilters({ ...filters, [target.value]: target.checked });
  }

  const handleSearchChange = (event) => {
    setSearch(event.target.value);
  };

  return (
    <PageList
      definition={definition}
      elements={elements}
      selectedId={eventId}
      texts={texts}
      newRole="time"
      sorting={sorting}
      setSorting={updateSorting}
      filters={
        <EventFilters
          statuses={filters}
          onStatusChange={handleStatusChange}
          search={search}
          onSearchChange={handleSearchChange}
        />
      }
    />
  );
}
